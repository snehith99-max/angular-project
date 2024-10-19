import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';

import { Subscription, Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { HttpClient } from '@angular/common/http';
import { saveAs } from 'file-saver';
import { NgxSpinnerService } from 'ngx-spinner';
import { ExcelService } from 'src/app/Service/excel.service';
import * as XLSX from 'xlsx';
import { Subject } from 'rxjs';
interface IProduct {
  product_gid: string;

}
@Component({
  selector: 'app-crm-mst-productsummary',
  templateUrl: './crm-mst-productsummary.component.html',
  styleUrls: ['./crm-mst-productsummary.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class CrmMstProductsummaryComponent {
  shopifyproduct_list:any;
  shopifyproduct:any;
  product!: IProduct;
  file!: File;
  image_path: any;
  imageData !: string;
  private unsubscribe: Subscription[] = [];
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  product_gid1:any;
  product_gid: any;
  products: any[] = [];
  response_data: any;
  product_count:any;
  image:any;
  showOptionsDivId:any;
  fileInputs: any;
  isExcelFile!: boolean;
  spinnerEnabled = false;
  data: any[] = [];
  constructor(private excelService : ExcelService, private fb: FormBuilder,private NgxSpinnerService: NgxSpinnerService,private http: HttpClient, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,) { }


  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetProductSummary();

    this.reactiveForm = new FormGroup({
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
      product_gid: new FormControl('')

    });
  }
  GetProductSummary() {

    var api = 'Product/GetProductSummary';
    this.service.get(api).subscribe((result: any) => {
      $('#product').DataTable().destroy();
      this.response_data = result;
      this.products = this.response_data.product_list;
      this.product_count = this.products[0].product_count;
      console.log('7uhjiio',this.products);
      setTimeout(() => {
        $('#product').DataTable();
      }, 1);
    });

  }
  
  toggleOptions(product_gid: any) {
    if (this.showOptionsDivId === product_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = product_gid;
    }
  }


  onChange1(event: any) {
    this.file = event.target.files[0];
    }

  onChange2(event: any) {
    this.file = event.target.files[0];
   
  }
  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Product Template";
    link.href = "assets/media/Excels/producttemplate/Product Template.xlsx";
    link.click();
  }
  downloadImage(data: any) {
  if(data.product_image !=null && data.product_image != "" ){
    saveAs(data.product_image, data.product_gid  + '.png');
  }
  else{
    window.scrollTo({
      top: 0, // Code is used for scroll top after event done
    });
    this.ToastrService.warning('No Image Found')

  }
  


  }
 
  downloadFile(file_path: string, file_name: string): void {
debugger
    const image = file_path.split('.net/');
    const page = image[0];
    const url = page.split('?');
    const imageurl = url[0];
    const parts = imageurl.split('.');
    const extension = parts.pop();

    this.service.downloadfile(imageurl, file_name+'.'+ extension).subscribe(
      (data: any) => {
        if (data != null) {
          this.service.filedownload1(data);
        } else {
          this.ToastrService.warning('Error in file download');
        }
      },
    );
  }

  onedit(params: any) {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.NgxSpinnerService.hide();
    this.router.navigate(['/crm/CrmMstProductEdit', encryptedParam])
  }
  onview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/crm/CrmMstProductView', encryptedParam])
  }
  importexcel(evt:any) {
    let header;
    const target: DataTransfer = <DataTransfer>(evt.target);
    this.isExcelFile = !! this.file.name.match(/(.xls|.xlsx)/); 
    
    if (this.isExcelFile) {

    debugger
    this.spinnerEnabled = true;
    const reader: FileReader = new FileReader();
 
    reader.onload = (e: any) => {
      /* read workbook */
      const bstr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });
 
      /* grab first sheet */
      const wsname: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsname];
 
      /* save data */
      this.data = XLSX.utils.sheet_to_json(ws);
    };
 
    reader.readAsBinaryString(this.file);
    reader.onloadend = (e) => {
      if (this.data && this.data.length > 0) {
        debugger
    let formData = new FormData();
     if (this.file != null && this.file != undefined) {
       window.scrollTo({
         top: 0, // Code is used for scroll top after event done
       });
       formData.append("file", this.file, this.file.name);
       this.NgxSpinnerService.show();
       var api = 'Product/ProductUploadExcels'
       this.service.postfile(api, formData).subscribe((result: any) => {
         this.responsedata = result;
         if (result.status == false) {
 
           this.NgxSpinnerService.hide();
           this.ToastrService.warning('Error While Occured Excel Upload')
         
    this.GetProductSummary();
         }
         else {
 
           this.NgxSpinnerService.hide();
           // window.location.reload();
           this.fileInputs= null;
           this.ToastrService.success("Excel Uploaded Successfully")
          
    this.GetProductSummary();

 
         }
         // this.NgxSpinnerService.hide();
         // this.ToastrService.success("Excel Uploaded Successfully");
         // window.location.reload();

       });
     }
    } else {
      // this.resetInputFile();
      this.ToastrService.warning('No data found in the Excel file');
     }
    };
    } else {
      //this.resetInputFile();
      alert("Please select a valid Excel file.");
    }
   }
  onadd() {
    this.router.navigate(['/crm/CrmMstProductAdd'])

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter


  }
 
  myModaladddetails(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveForm.get("product_gid")?.setValue(this.parameterValue1.product_gid);   

  }
  public onsubmit(): void {
    console.log(this.reactiveForm.value)
    this.product = this.reactiveForm.value;
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("product_gid", this.product.product_gid);
      this.NgxSpinnerService.show();
      var api7 = 'Product/GetProductImage'
      this.service.postfile(api7, formData).subscribe((result: any) => {
        if(result.status ==false){
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else{
          // this.router.navigate(['/crm/CrmMstProductsummary']);
          this.NgxSpinnerService.hide();
          this.ToastrService.success(result.message)

          this.GetProductSummary();
        }

        this.responsedata = result;
       

      });
      
    }
  }

  exportExcel() :void {
    const ProductList = this.products.map(item => ({
      Product_Type: item.producttype_name || '',
      Product_Group: item.productgroup_name || '',
      Product_Code: item.product_code || '',
      Product: item.product_name || '',
      Unit: item.productuomclass_name || '',
     
     }));     
      // this.excelService.exportAsExcelFile(ProductList , 'Product');
       // Create a new table element
  const table = document.createElement('table');

  // Add header row with background color
  const headerRow = table.insertRow();
  Object.keys(ProductList[0]).forEach(header => {
    const cell = headerRow.insertCell();
    cell.textContent = header;
    cell.style.backgroundColor = '#00317a'; 
    cell.style.color = '#FFFFFF';
    cell.style.fontWeight = 'bold';
    cell.style.border = '1px solid #000000';
  });

  // Add data rows
  ProductList.forEach(item => {
    const dataRow = table.insertRow();
    Object.values(item).forEach(value => {
      const cell = dataRow.insertCell();
      cell.textContent = value;
      cell.style.border = '1px solid #000000';
    });
  });

  // Convert the table to a data URI
  const tableHtml = table.outerHTML;
  const dataUri = 'data:application/vnd.ms-excel;base64,' + btoa(unescape(encodeURIComponent(tableHtml)));

  // Trigger download
  const link = document.createElement('a');
  link.href = dataUri;
  link.download = 'Product.xls';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
    }
  ondelete() {
    // console.log(this.parameterValue);
    this.NgxSpinnerService.show();
    var url = 'Product/Getdeleteproductdetails'
    let param = {
      product_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({

 

          top: 0, // Code is used for scroll top after event done



        });
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({

 

          top: 0, // Code is used for scroll top after event done



        });
       


        this.ToastrService.success(result.message);
        
      this.GetProductSummary();


      }


    });
  }

  toggleStatus(data: any) {

    if (data.statuses === 'Active') {
      this.openModalinactive(data);
    } else {
      this.openModalactive(data);
    }
  }


  openModalinactive(parameter: string){
    this.parameterValue = parameter
  }
  
  
  oninactive(){
    console.log(this.parameterValue);
      var url3 = 'Product/GetcustomerInactive'
      this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning('Error While Product Inactivated')
        }
        else {
         this.ToastrService.success('Product Inactivated Successfully')
          }
        window.location.reload();
      });
  }
  
  openModalactive(parameter: string){
    this.parameterValue = parameter
  }
  
  onactive(){
    console.log(this.parameterValue);
      var url3 = 'Product/GetcustomerActive'
      this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning('Error While Customer Activated')
        }
        else {
         this.ToastrService.success('Customer Activated Successfully')
          }
        window.location.reload();
      });
  }
  
  openImagePopup(imageUrl: string): void {
    const modal = document.getElementById('imageModal') as HTMLElement;
    const modalImg = document.getElementById('modalImage') as HTMLImageElement;

    modalImg.src = imageUrl;

    modal.style.display = "block";
  }

  closeImagePopup(): void {
    const modal = document.getElementById('imageModal') as HTMLElement;
    modal.style.display = "none";
  }
  onclose() {
    this.fileInputs= null;
  }
  canceluploadexcel(){
    this.fileInputs= null;
  }
}

