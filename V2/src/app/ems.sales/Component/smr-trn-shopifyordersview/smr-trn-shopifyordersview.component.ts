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
@Component({
  selector: 'app-smr-trn-shopifyordersview',
  templateUrl: './smr-trn-shopifyordersview.component.html',
  styleUrls: ['./smr-trn-shopifyordersview.component.scss']
})
export class SmrTrnShopifyordersviewComponent {
  salesorder_gid: any;
  responsedata: any;
  ViewsalesSummary_list: any;
  shopifyorder_number: any;
  shopifyproductordersummary_list: any;




  constructor(private fb: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private http: HttpClient, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,) { }


  ngOnInit(): void {
    debugger
    const salesorder_gid = this.route.snapshot.paramMap.get('salesorder_gid');
    this.salesorder_gid = salesorder_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.salesorder_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetViewsalesSummary(deencryptedParam)
  }


  GetViewsalesSummary(salesorder_gid: any) {
    var url = 'ShopifyCustomer/ViewShopifyOrderSummary'
    let param = {
      salesorder_gid: salesorder_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.ViewsalesSummary_list = result.shopifyordersummary_list;
      console.log(this.ViewsalesSummary_list);
      this.shopifyorder_number = this.ViewsalesSummary_list[0].shopifyorder_number;
    });
    this.GetViewsalesproductSummary(salesorder_gid);
  }
  GetViewsalesproductSummary(salesorder_gid: any) {
    var url = 'ShopifyCustomer/GetViewsalesproductSummary'
    let param = {
      salesorder_gid: salesorder_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.shopifyproductordersummary_list = result.shopifyproductordersummary_list;
      setTimeout(() => {
        $('#shopifyproductordersummary_list').DataTable();
      }, 1);
    });
  }
  onsubmitorder() {
debugger
    let param = {
      salesorder_gid:  this.ViewsalesSummary_list[0].salesorder_gid,
      customer_address:  this.ViewsalesSummary_list[0] .customer_address,
      Grandtotal:  this.ViewsalesSummary_list[0].Grandtotal,
      customer_contact_person:  this.ViewsalesSummary_list[0].customer_contact_person,
      item_count:  this.ViewsalesSummary_list[0].item_count,
      message:  this.ViewsalesSummary_list[0].message,
      salesorder_date:  this.ViewsalesSummary_list .salesorder_date,
      salesorder_status:  this.ViewsalesSummary_list[0].salesorder_status,
      shopify_orderid:  this.ViewsalesSummary_list[0].shopify_orderid,
      shopifyorder_number:  this.ViewsalesSummary_list [0].shopifyorder_number,
    }
    this.NgxSpinnerService.show();
    var url1 = 'ShopifyCustomer/Postshopifyacceptorder'
    this.service.post(url1, param).pipe().subscribe((result: any) => {

      if (result.status == false) {

        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Error While Occured Accepting Order');
      }
      else {
        this.NgxSpinnerService.hide();
        this.ToastrService.success('Order Accepted Sucessfully !');
        this.router.navigate(['/smr/SmrTrnShopifyorders'])
      }

    });

  }
}
