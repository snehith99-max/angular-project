import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Subscription, Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { ExcelService } from 'src/app/Service/excel.service';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-smr-mst-producthsncode',
  templateUrl: './smr-mst-producthsncode.component.html',
  styleUrls: ['./smr-mst-producthsncode.component.scss']
})
export class SmrMstProducthsncodeComponent {

  response_data: any;
  products: any[] = [];
  productform: FormGroup | any;
  temptable: any[] = [];




  constructor(private fb: FormBuilder, private sanitizer: DomSanitizer, private excelService: ExcelService, public NgxSpinnerService: NgxSpinnerService, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService) {
    this.productform = new FormGroup({
      product_hsncode: new FormControl(''),
      product_hsncode_desc: new FormControl(''),
      product_hsngst: new FormControl('')
    })
  }


  ngOnInit(): void {
    this.GetProductSummary();

    this.productform = new FormGroup({
      product_hsncode: new FormControl(''),
      product_hsncode_desc: new FormControl(''),
      product_hsngst: new FormControl('')
    })
  }
  GetProductSummary() {
debugger
    var api = 'SmrProductHsnCode/ProductSummary';
    this.NgxSpinnerService.show()
    this.service.get(api).subscribe((result: any) => {
      $('#product_list').DataTable().destroy();
      this.response_data = result;
      this.products = this.response_data.ProductHsnCode_summary;
     
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#product_list').DataTable();

      }, 1);
    });

  }



  HsnCodeUpdate(product_gid:any,hsn_number:any,hsn_desc: any,gstproducttax_percentage:any) {
    const api = 'SmrProductHsnCode/UpdateProductHSNCode';
    const param = {
      product_gid:product_gid,
      product_hsngst : gstproducttax_percentage,
      product_hsncode_desc : hsn_desc,
      product_hsncode : hsn_number
     
    };
    console.log(param)
    this.service.getparams(api, param).subscribe((result: any) => {
      if (result.status === false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
        this.GetProductSummary();
      }
    });
  }
}
