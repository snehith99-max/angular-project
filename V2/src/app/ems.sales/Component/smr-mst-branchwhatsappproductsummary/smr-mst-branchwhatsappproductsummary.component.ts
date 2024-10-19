import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';

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
  selector: 'app-smr-mst-branchwhatsappproductsummary',
  templateUrl: './smr-mst-branchwhatsappproductsummary.component.html',
  styleUrls: ['./smr-mst-branchwhatsappproductsummary.component.scss']
})
export class SmrMstBranchwhatsappproductsummaryComponent {
  shopifybranchproduct_list: any;
  shopifyproduct: any;
  branchgid: any;
  branch_gid: any;
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
  constructor(private excelService: ExcelService,
     private fb: FormBuilder, 
     private NgxSpinnerService: NgxSpinnerService,
      private http: HttpClient,
       private route: ActivatedRoute,
        private router: Router, 
        private service: SocketService, 
        private ToastrService: ToastrService,) { }


  ngOnInit(): void {
debugger
    
    this.branchgid = this.route.snapshot.paramMap.get('branch_gid');
    const key = 'storyboard';
    this.branch_gid = AES.decrypt(this.branchgid,key).toString(enc.Utf8);
    this.GetProductSummary(this.branch_gid);
    

    this.reactiveForm = new FormGroup({
      file: new FormControl(''),
      fileExtension: new FormControl(''),
      fileName: new FormControl(''),
      imagePath: new FormControl(''),
      product_gid: new FormControl('')

    });
  }
  GetProductSummary(branch_gid:any) {
let param = {branch_gid  : branch_gid}
    var api = 'SmrMstWhatsappproductpricemanagement/GetbranchwhatsappProductSummary';
    this.service.getparams(api, param).subscribe((result: any) => {
      $('#product').DataTable().destroy();
      this.response_data = result;
      this.products = this.response_data.branchproduct_list;
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
          this.GetProductSummary(this.branch_gid);
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
    var url = 'SmrMstWhatsappproductpricemanagement/BranchAddproducttowhatsapp'
    let param = {
      product_gid: this.parameterValue,
      branch_gid:this.branch_gid
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
      this.GetProductSummary(this.branch_gid);
    });
  }
  public toggleswitch(param: any): void {
    this.NgxSpinnerService.show();
    var api = 'SmrMstWhatsappproductpricemanagement/Branchupdatewhatsappstockstatus'
    this.service.post(api, param).subscribe((result: any) => {
      if (result.status == false) {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0, // Code is used for scroll top after event done
        });
        this.ToastrService.warning(result.message)
        this.GetProductSummary(this.branch_gid);
      }
      else {
        this.NgxSpinnerService.hide();
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.GetProductSummary(this.branch_gid);
      }
    });
  }
  onremove() {
    this.NgxSpinnerService.show();
    var url = 'SmrMstWhatsappproductpricemanagement/BranchRemoveproductfromwt'
    let param = {
      whatsapp_id: this.parameterValue,
      branch_gid:this.branch_gid
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
      this.GetProductSummary(this.branch_gid);
    });
  }
}
