import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { SelectionModel } from '@angular/cdk/collections';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { AES } from 'crypto-js';
import { saveAs } from 'file-saver';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
export class IShopifyProduct {
  shopifyproduct_lists: string[] = [];
}
interface IProductPrice {

  shopify_productid: string;
  price: string;
  variant_id: string;
  attribute: string;
}
@Component({
  selector: 'app-smr-trn-shopifyproduct',
  templateUrl: './smr-trn-shopifyproduct.component.html',
  styleUrls: ['./smr-trn-shopifyproduct.component.scss']
})
export class SmrTrnShopifyproductComponent {
  shopifyordercountsummary_list: any;
  product_count: any;
  currentId: number = 0;
  pick: Array<any> = [];
  CurObj1: IShopifyProduct = new IShopifyProduct();
  selection1 = new SelectionModel<IShopifyProduct>(true, []);
  selection6 = new SelectionModel<IShopifyProduct>(true, []);
  selection5 = new SelectionModel<IShopifyProduct>(true, []);
  shopifyproduct: any;
  shopifyproduct_list: any;
  response_data: any;
  products: any;
  sync_shopify: boolean = false;
  syncshopify_list: any;
  syncshopify_listcount: any;
  sync_crm: boolean = false;
  sync_crmlist: any;
  sync_crmlistcount: any;
  emptyFlag1: boolean = true;
  shopifyproductid: any;
  parameterValue1: any;
  reactiveFormPrice!: FormGroup;
  productprice!: IProductPrice;
  file!: File;
  responsedata: any;
  sellersPermitFile: any;
  DriversLicenseFile: any;
  InteriorPicFile: any;
  ExteriorPicFile: any;

  sellersPermitString!: string;
  DriversLicenseString!: string;
  InteriorPicString!: string;
  ExteriorPicString!: string;
  parameterValue: any;
  showOptionsDivId:any;
  constructor(public service: SocketService, private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService
    , private router: Router){
      this.productprice = {} as IProductPrice;
  }
  ngOnInit(): void {
    this.reactiveFormPrice = new FormGroup({
      price: new FormControl(this.productprice.price, [
        Validators.required, Validators.maxLength(10),
      ]),
      attribute: new FormControl(this.productprice.attribute, [
        Validators.required, Validators.maxLength(36),
      ]),
      shopify_productid: new FormControl(),
      variant_id: new FormControl(),
    });


    var url3 = 'ShopifyCustomer/GetShopifyOrderCountSummary'
    this.service.get(url3).subscribe((result,) => {

      this.shopifyordercountsummary_list = result;
      this.product_count = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].product_count;
      
    });
    this.GetProductSummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  }
  get price() {
    return this.reactiveFormPrice.get('price')!;
  }
  get attribute() {
    return this.reactiveFormPrice.get('attribute')!;
  }
  public picked(event: any, field: any) {
    this.currentId = field;
    let fileList: FileList = event.target.files;
    event = null;
    if (fileList.length > 0) {
      const file: File = fileList[0];
      if (field == 1) {
        this.sellersPermitFile = file;
        this.handleInputChange(file); //turn into base64
      }
      else if (field == 2) {
        this.DriversLicenseFile = file;
        this.handleInputChange(file); //turn into base64
      }
      else if (field == 3) {
        this.InteriorPicFile = file;
        this.handleInputChange(file); //turn into base64
      }
      else if (field == 4) {
        this.ExteriorPicFile = file;
        this.handleInputChange(file); //turn into base64

      }
    }
    else {
      // alert("No file selected");
      this.ToastrService.warning("No file selected")
    }
  }
  public onuploadimage(): void {
    let param = {
      shopify_productid: this.shopifyproductid,
      path: this.sellersPermitString
    }
    if (this.sellersPermitString != null && this.sellersPermitString != undefined) {
      this.NgxSpinnerService.show();
      var url = 'Product/UpdateShopifyProductImage'
      this.service.postparams(url, param).subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.warning(result.message)
        }
        else {
          // this.router.navigate(['/crm/CrmMstProductsummary']);
          this.NgxSpinnerService.hide();
          window.scrollTo({
            top: 0, // Code is used for scroll top after event done
          });
          this.ToastrService.success(result.message)
          this.GetProductSummary();
          // window.location.reload();
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select File !")
    }
  }
  GetProductSummary() {
    var api = 'Product/GetShopifyProductSummary';
    this.service.get(api).subscribe((result: any) => {
      $('#product').DataTable().destroy();
      this.response_data = result;
      this.products = this.response_data.product_list;
      setTimeout(() => {
        $('#product').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
    var url = 'Product/GetShopifyProductdetails'
    this.service.get(url).subscribe((result: any) => {
      this.shopifyproduct = result;
      this.shopifyproduct_list = this.shopifyproduct.products;
    });


    // var url3 = 'ShopifyCustomer/GetShopifyOrderCountSummary'
    // this.service.get(url3).subscribe((result,) => {
    //   this.shopifyordercountsummary_list = result;
    //   this.product_count = this.shopifyordercountsummary_list.shopifyordercountsummary_list[0].product_count;
    //   });
  }
  onedit(params: any) {
    this.NgxSpinnerService.show();
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/crm/CrmSmmShopifyProductEdit', encryptedParam])
    this.NgxSpinnerService.hide();
  }
  onsubmitproduct() {

    this.pick = this.selection1.selected
    let list = this.pick
    this.CurObj1.shopifyproduct_lists = list
    if (this.CurObj1.shopifyproduct_lists.length != 0) {
      //console.log(this.CurObj1)
      this.NgxSpinnerService.show();
      var url1 = 'Product/Sendproductmaster'
      this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Moving Product');
        }
        else {
          this.GetProductSummary();
          this.NgxSpinnerService.hide();
          this.selection1.clear();
          this.ToastrService.success('Product Moved Sucessfully!')
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record to Move Product! ")
    }
  }
  onadd() {
    this.router.navigate(['/crm/CrmSmmShopifyProductAdd'])

  }
  masterToggle1() {
    this.isAllSelected1() ?
      this.selection1.clear() :
      this.products.forEach((row: IShopifyProduct) => this.selection1.select(row));
  }
  isAllSelected1() {
    const numSelected = this.selection1.selected.length;
    const numRows = this.products.length;
    return numSelected === numRows;
  }
  isAllSelected6() {
    const numSelected = this.selection6.selected.length;
    const numRows = this.sync_crmlist.length;
    return numSelected === numRows;
  }

  masterToggle6() {
    this.isAllSelected6() ?
      this.selection6.clear() :
      this.sync_crmlist.forEach((row: IShopifyProduct) => this.selection6.select(row));
  }
  isAllSelected5() {
    const numSelected = this.selection5.selected.length;
    const numRows = this.syncshopify_list.length;
    return numSelected === numRows;
  }

  masterToggle5() {
    this.isAllSelected5() ?
      this.selection5.clear() :
      this.syncshopify_list.forEach((row: IShopifyProduct) => this.selection5.select(row));
  }
  Getsync_shopify() {
    this.sync_shopify = true;
    this.Getsyncshopify();

  }
  Getsync_crm() {
    this.sync_crm = true;
    this.Getsynccrm();

  }
  Getbacksync_crm() {
    this.sync_crm = false;
    this.GetProductSummary();

  }
  Getbacksync_shopify() {
    this.sync_shopify = false;
    this.GetProductSummary();

  }
  myModaladddetails(parameter: string) {
    //console.log(parameter)
    this.shopifyproductid = parameter;

    this.emptyFlag1 = true;
  }
  Getsyncshopify() {

    var api = 'Product/Getsyncshopify';
    this.service.get(api).subscribe((result: any) => {
      $('#syncshopify_list').DataTable().destroy();
      this.response_data = result;
      this.syncshopify_list = this.response_data.product_list;
      this.syncshopify_listcount = this.response_data.product_list.length;

      setTimeout(() => {
        $('#syncshopify_list').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  Getsynccrm() {
    var api = 'Product/Getsync_crm';
    this.service.get(api).subscribe((result: any) => {
      $('#sync_crmlist').DataTable().destroy();
      this.response_data = result;
      this.sync_crmlist = this.response_data.product_list;
      this.sync_crmlistcount = this.response_data.product_list.length;

      setTimeout(() => {
        $('#sync_crmlist').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });
  }
  downloadImage(data: any) {
    if (data.product_image != null && data.product_image != "" && data.product_image != "0" && data.product_image != 0 && data.product_image != "no") {

      saveAs(data.product_image, data.product_gid + '.png');
    }
    else {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      this.ToastrService.warning('No Image Found')
    }
  }
  updateprice(data: any) {
    // console.log(data);
     this.parameterValue1 = data
 
     this.reactiveFormPrice.get("price")?.setValue(this.parameterValue1.price);
     this.reactiveFormPrice.get("shopify_productid")?.setValue(this.parameterValue1.shopify_productid);
     this.reactiveFormPrice.get("variant_id")?.setValue(this.parameterValue1.variant_id);
     this.reactiveFormPrice.get("attribute")?.setValue(this.parameterValue1.option1);
   }
   downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Product";
    link.href = "assets/media/Excels/UPLF23092048.xlsx";
    link.click();
  }
  onChange1(event: any) {
    this.file = event.target.files[0];
  }
  onChange2(event: any) {
    this.file = event.target.files[0];
  }
  importexcel() {
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {

      formData.append("file", this.file, this.file.name);

      var api = 'Product/ProductUploadExcels'

      this.service.postfile(api, formData).subscribe((result: any) => {
        this.responsedata = result;

        // this.router.navigate(['/crm/CrmMstProductsummary']);
        window.location.reload();
        this.ToastrService.success("Excel Uploaded Successfully")

      });

    }
  }
  handleInputChange(files: any) {
    var file = files;
    var pattern = /image-*/;
    var reader = new FileReader();
    if (!file.type.match(pattern)) {
      this.ToastrService.warning("Invalid Format")
      // alert('invalid format');
      this.emptyFlag1 = false;
      return;
    }
    this.emptyFlag1 = true;
    reader.onloadend = this._handleReaderLoaded.bind(this);
    reader.readAsDataURL(file);
  }
  _handleReaderLoaded(e: any) {
    let reader = e.target;
    var base64result = reader.result.substr(reader.result.indexOf(',') + 1);
    //this.imageSrc = base64result;
    let id = this.currentId;
    switch (id) {
      case 1:
        this.sellersPermitString = base64result;
        break;
      case 2:
        this.DriversLicenseString = base64result;
        break;
      case 3:
        this.InteriorPicString = base64result;
        break;
      case 4:
        this.ExteriorPicString = base64result;
        break
    }
  }
  oncloseimage() {

  }
  onclose() {

  }
  onupdateprice() {
    //console.log(this.reactiveFormPrice.value)
    this.NgxSpinnerService.show();
    var url = 'Product/UpdateShopifyProductPrice'

    this.service.post(url, this.reactiveFormPrice.value).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning(result.message)
        this.GetProductSummary();
        this.reactiveFormPrice.reset();

      }
      else {
        this.ToastrService.success(result.message)
        this.GetProductSummary();
        this.reactiveFormPrice.reset();

      }
      this.NgxSpinnerService.hide();
    });
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
        this.ToastrService.success(result.message)
      }
      this.GetProductSummary();
    });
  }
  Postsynccrm() {

    this.pick = this.selection6.selected
    let list = this.pick
    this.CurObj1.shopifyproduct_lists = list
    if (this.CurObj1.shopifyproduct_lists.length != 0) {
      //console.log(this.CurObj1)
      this.NgxSpinnerService.show();
      var url1 = 'Product/Postsynccrm'
      this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {

        if (result.status == false) {

          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Moving Product');
        }
        else {
          this.GetProductSummary();
          this.NgxSpinnerService.hide();
          this.selection1.clear();
          this.ToastrService.success('Product Moved Sucessfully!')
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record to Move Product! ")
    }
  }
  Postsyncshopify() {

    this.pick = this.selection5.selected
    let list = this.pick
    this.CurObj1.shopifyproduct_lists = list
    if (this.CurObj1.shopifyproduct_lists.length != 0) {
      //console.log(this.CurObj1)
      this.NgxSpinnerService.show();
      var url1 = 'Product/Sendproductmaster'
      this.service.post(url1, this.CurObj1).pipe().subscribe((result: any) => {
        if (result.status == false) {
          this.NgxSpinnerService.hide();
          this.ToastrService.warning('Error While Occured Moving Product');
        }
        else {
          this.GetProductSummary();
          this.NgxSpinnerService.hide();
          this.selection1.clear();

          this.ToastrService.success('Product Moved Sucessfully!')
        }
      });
    }
    else {
      this.ToastrService.warning("Kindly Select Atleast One Record to Move Product! ")
    }
  }
  toggleOptions(shopify_productid: any) {
    if (this.showOptionsDivId === shopify_productid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = shopify_productid;
    }
  }
}
