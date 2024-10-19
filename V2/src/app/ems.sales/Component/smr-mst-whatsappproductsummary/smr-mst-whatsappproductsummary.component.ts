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
interface IProduct {
  product_gid: string;

}
@Component({
  selector: 'app-smr-mst-whatsappproductsummary',
  templateUrl: './smr-mst-whatsappproductsummary.component.html',
  styleUrls: ['./smr-mst-whatsappproductsummary.component.scss'],
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
export class SmrMstWhatsappproductsummaryComponent {
  shopifyproduct_list: any;
  shopifyproduct: any;
  product!: IProduct;
  file!: File;
  image_path: any;
  imageData !: string;
  private unsubscribe: Subscription[] = [];
  reactiveForm!: FormGroup;
  responsedata: any;
  parameterValue: any;
  parameterValue1: any;
  product_gid1: any;
  product_gid: any;
  products: any[] = [];
  response_data: any;
  product_count: any;
  image: any;
  showOptionsDivId: any;
  whatsapp_id: any;
  active_Products: any;
  product_added: any;
  in_stock: any;
  out_of_stock: any;
  constructor(private excelService: ExcelService, private fb: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private http: HttpClient, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,) { }


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

    var api = 'Product/GetwhatsappProductSummary';
    this.service.get(api).subscribe((result: any) => {
      $('#product').DataTable().destroy();
      this.response_data = result;
      this.products = this.response_data.product_list;
      this.active_Products = result.active_Products
      this.product_added = result.product_added
      this.in_stock = result.in_stock
      this.out_of_stock = result.out_of_stock

      console.log('7uhjiio', this.products);
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
    link.download = "Product";
    link.href = "assets/media/Excels/UPLF23092048.xlsx";
    link.click();
  }
  downloadImage(data: any) {
    if (data.product_image != null && data.product_image != "") {
      saveAs(data.product_image, data.product_gid + '.png');
    }
    else {
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

    this.service.downloadfile(imageurl, file_name + '.' + extension).subscribe(
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
    this.router.navigate(['/smr/SmrMstWhatsappproductedit', encryptedParam])
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
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning(result.message)
        }
        else {
          // this.router.navigate(['/crm/CrmMstProductsummary']);
          this.NgxSpinnerService.hide();
          this.GetProductSummary();
          this.ToastrService.success(result.message)
          window.location.reload();

        }

        this.responsedata = result;


      });

    }
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
  whatsappproduct(parameter: string) {
    this.parameterValue = parameter
  }
  removeproduct(parameter: string) {
    this.parameterValue = parameter
  }

  addwhatsappproduct() {
    this.NgxSpinnerService.show();
    var url = 'Product/Addproducttowhatsapp'
    let param = {
      product_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)

      }
      this.GetProductSummary();
    });
  }
  public toggleswitch(param: any): void {
    this.NgxSpinnerService.show();
    var api = 'Product/updatewhatsappstockstatus'
    this.service.post(api, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.GetProductSummary();
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.GetProductSummary();
      }
    });
  }
  onremove() {
    this.NgxSpinnerService.show();
    var url = 'Product/Removeproductfromwt'
    let param = {
      whatsapp_id: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({ top: 0, });
        this.ToastrService.warning(result.message)
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
      }
      this.GetProductSummary();
    });
  }
}
