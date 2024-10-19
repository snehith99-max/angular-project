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

export class unAssign {
  productgroup_gid: any;
  productgroup_name: any;
  product_gid: any;
  product_name: any;
  productuom_gid: any;
  productuom_name: any;
  sku: any;
  product_desc:any;
  mrp:any;
  productunassign_list:any;
  product_code:any;
  pricesegment_gid:any;
 
}

@Component({
  selector: 'app-smr-mst-priceunassignproduct',
  templateUrl: './smr-mst-priceunassignproduct.component.html',
  styleUrls: ['./smr-mst-priceunassignproduct.component.scss']
})
export class SmrMstPriceunassignproductComponent {

productunassign_list:any[]=[];
selection = new SelectionModel<unAssign>(true, []);
ProductForm!: FormGroup;
productunassign:any;
pick:Array<any> = [];
CurObj: unAssign = new unAssign();

  constructor(private http: HttpClient,public NgxSpinnerService:NgxSpinnerService, private fb: FormBuilder, private router: ActivatedRoute, private route: Router, private service: SocketService, private ToastrService: ToastrService) {
  }
  ngOnInit():void{
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

    this.productunassign = this.router.snapshot.paramMap.get('pricesegment_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.productunassign, secretKey).toString(enc.Utf8);
    this.GetSmrMstProductunAssignSummary(deencryptedParam);

    
    this.GetSmrMstProductunAssignSummary(deencryptedParam);
  }
  GetSmrMstProductunAssignSummary(pricesegment_gid:any){
    let param = {
      pricesegment_gid: pricesegment_gid,

    }
    var url = 'SmrMstPricesegmentSummary/GetSmrMstProductunAssignSummary'
    this.NgxSpinnerService.show()
    this.service.getparams(url, param).subscribe((result: any) => {
      $('#productunassign_list').DataTable().destroy();
    
      this.productunassign_list = result.unassignproduct_list
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#productunassign_list').DataTable();
      }, 1);

    });
  }
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.productunassign_list.length;
    return numSelected === numRows;
  }
  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.productunassign_list.forEach((row: unAssign) => this.selection.select(row));
  }
  onsubmit() {
    this.productunassign = this.router.snapshot.paramMap.get('pricesegment_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.productunassign, secretKey).toString(enc.Utf8);
    this.pick = this.selection.selected  
    this.CurObj.productunassign_list = this.pick
    this.CurObj.pricesegment_gid=deencryptedParam
      if (this.CurObj.productunassign_list.length === 0) {
      this.ToastrService.warning("Select atleast one product");
      return;
    } 
    else{

      var url = 'SmrMstPricesegmentSummary/UnAssignProduct'
      this.NgxSpinnerService.show()
      this.service.post(url, this.CurObj).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetSmrMstProductunAssignSummary(deencryptedParam);
          this.selection.clear();
          this.NgxSpinnerService.hide()


        }
        else {
          

          this.ToastrService.success(result.message)
          this.ProductForm.reset();
          this.route.navigate(['/smr/SmrMstPricesegmentSummary'])
          this.GetSmrMstProductunAssignSummary(deencryptedParam);
          this.NgxSpinnerService.hide()
          



        }


      });
    }

    }
    back() {
      this.route.navigate(['/smr/SmrMstPricesegmentSummary']);
    }

}
