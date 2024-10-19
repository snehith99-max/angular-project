import { Component } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { SelectionModel } from '@angular/cdk/collections';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { HttpClient } from '@angular/common/http';

export class Assign {
  productgroup_gid: any;
  productgroup_name: any;
  product_gid: any;
  product_name: any;
  productuom_gid: any;
  productuom_name: any;
  sku: any;
  product_desc:any;
  mrp:any;
  productgroup_list:any;
  product_code:any;
  pricesegment_gid:any;
 
}

@Component({
  selector: 'app-smr-mst-product-assign',
  templateUrl: './smr-mst-product-assign.component.html',
  styleUrls: ['./smr-mst-product-assign.component.scss']
})
export class SmrMstProductAssignComponent {

  productArray: any[] = [];
  productgroup_list: any[] = [];
  producthead_list: any[] = [];
  ProductForm!: FormGroup;
  product_list: any[] = [];
  productdetails_list: any[] = [];
  selection = new SelectionModel<Assign>(true, []);
  productuom_list: any[] = [];
  group_list: any[] = [];
  head_list: any[] = [];

  mdlProductName: any;
  responsedata: any;
  productasgn: any;
  data: any;
  mdlProductUnitName: any;
  mdlProducGrouptName: any;
  mdlProductUom: any;
  parameterValue: any;
  originalValues: any[] = [];
  pick:Array<any> = [];
  CurObj: Assign = new Assign();

  constructor(private http: HttpClient, private fb: FormBuilder,public NgxSpinnerService:NgxSpinnerService, private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService) {
  }
  ngOnInit(): void {
    debugger
    //this.LoadAllProducts();
    this.ProductForm = new FormGroup({
      product_gid: new FormControl(''),
      product_name: new FormControl(''),
      productgroup_gid: new FormControl(''),
      productgroup_name: new FormControl(''),
      pricesegment_gid: new FormControl(''),

      productuom_gid: new FormControl(''),
      productuom_name: new FormControl(''),
      // customerproduct_code: new FormControl(''),
      product_price: new FormControl(''),

    });


    this.productasgn = this.router.snapshot.paramMap.get('pricesegment_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.productasgn, secretKey).toString(enc.Utf8);
    this.GetSmrMstProductAssignSummary(deencryptedParam);
  }
  
  
  GetSmrMstProductAssignSummary(pricesegment_gid: any) {

   
    var url = 'SmrMstPricesegmentSummary/GetSmrMstProductAssignSummary';
    this.NgxSpinnerService.show()
    let param = {
      pricesegment_gid: pricesegment_gid,

    }
   
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.responsedata = result;
      this.productgroup_list = result.productgroup_list
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#productgroup_list').DataTable();
      }, 1);

    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.productgroup_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.productgroup_list.forEach((row: Assign) => this.selection.select(row));
  }



  update() {
    
    var url = 'SmrMstPricesegmentSummary/GetUnAssignProduct'
    this.NgxSpinnerService.show()
    let param = {
      product_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();
      }
      else {

        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();

      }


    });

  }
  openModaledit(data: any) {
    this.productgroup_list.forEach(element => {
      element.isEdit = false;
      data.originalProductPrice = data.product_price;

    });
    data.isEdit = true;



  }
  edupdate() {
    this.productasgn = this.router.snapshot.paramMap.get('pricesegment_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.productasgn, secretKey).toString(enc.Utf8);

    this.pick = this.selection.selected  
    this.CurObj.productgroup_list = this.pick
    this.CurObj.pricesegment_gid=deencryptedParam
      if (this.CurObj.productgroup_list.length === 0) {
      this.ToastrService.warning("Select atleast one product");
      return;
    } 

    var url = 'SmrMstPricesegmentSummary/GetUpdateProduct'
    
    this.service.postparams(url, this.CurObj).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
      window.location.reload()
    });

  }


 
  get product_name() {
    return this.ProductForm.get('product_name')!;
  }
  get product_price() {
    return this.ProductForm.get('product_price')!;
  }

  onsubmit() {
    this.productasgn = this.router.snapshot.paramMap.get('pricesegment_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.productasgn, secretKey).toString(enc.Utf8);
    this.pick = this.selection.selected  
    this.CurObj.productgroup_list = this.pick
    this.CurObj.pricesegment_gid=deencryptedParam
      if (this.CurObj.productgroup_list.length === 0) {
      this.ToastrService.warning("Select atleast one product");
      return;
    } 
    else{

      var url = 'SmrMstPricesegmentSummary/PostProduct'
      this.NgxSpinnerService.show();
      this.service.post(url, this.CurObj).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetSmrMstProductAssignSummary(deencryptedParam);
          this.NgxSpinnerService.hide();
          this.selection.clear();

        }
        else {
          

          this.ToastrService.success(result.message)
          this.ProductForm.reset();
          this.route.navigate(['/smr/SmrMstPricesegmentSummary'])
          this.GetSmrMstProductAssignSummary(deencryptedParam);
          this.NgxSpinnerService.hide();



        }


      });
    }

    }
    
  onclose(data: any) {
    if (data.isEdit) {
      data.isEdit = false;
    }
  }
  back() {
    this.route.navigate(['/smr/SmrMstPricesegmentSummary']);
  }


}
