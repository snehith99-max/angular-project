import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
@Component({
  selector: 'app-otl-trn-whatsapporderview',
  templateUrl: './otl-trn-whatsapporderview.component.html',
})
export class OtlTrnWhatsapporderviewComponent {
  kot_gid: any;
  Viewwhatsappsummary_list: any;
  order_id: any;
  kot_tot_price: number = 0;
  kot_delivery_charges: number = 0;
  source: any;
  message_id: any;
  order_type: any;
  created_date: any;
  payment_status: any;
  branch_name: any;
  customer_phone: any;
  address: any;
  deencryptedParam: any;
  reject_reason: any;
  line_items_total: any;
  order_instructions: any;
  constructor(private formBuilder: FormBuilder, public ToastrService: ToastrService, private route: Router, private router: ActivatedRoute, public service: SocketService) { }

  ngOnInit(): void {
    const kot_gid = this.router.snapshot.paramMap.get('kot_gid');
    this.kot_gid = kot_gid;
    const secretKey = 'storyboarderp';
    this.deencryptedParam = AES.decrypt(this.kot_gid, secretKey).toString(enc.Utf8);
    this.GetViewwhatsapporderSummary(this.deencryptedParam);
  }
  GetViewwhatsapporderSummary(kot_gid: any) {
    var url = 'OtlWhatsAppOrder/GetViewwhatsapporderSummary'
    let param = {
      kot_gid: kot_gid
    }

    this.service.getparams(url, param).subscribe((result: any) => {
      this.Viewwhatsappsummary_list = result.Viewwhatsappsummary_list;
      this.order_id = result.order_id
      this.kot_tot_price = result.kot_tot_price;
      this.kot_delivery_charges = result.kot_delivery_charges;
      this.source = result.source;
      this.message_id = result.message_id;
      this.order_type = result.order_type;
      this.created_date = result.created_date;
      this.payment_status = result.payment_status;
      this.branch_name = result.branch_name;
      this.customer_phone = result.customer_phone;
      this.address = result.address;
      this.reject_reason = result.reject_reason;
      this.line_items_total = result.line_items_total;
      this.order_instructions = result.order_instructions;




    });
  }
  onBack() {
    this.route.navigate(['/outlet/OtlTrnWhatsAppOrderSummary']);
  }
  onapprovewhatapporder() {
    var url = "OtlWhatsAppOrder/onapprovewhatapporder";
    let param = {
      kot_gid: this.deencryptedParam
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.route.navigate(['/outlet/OtlTrnWhatsAppOrderSummary']);
      }
      else {
        this.ToastrService.warning(result.message)
      }
    })
  }

}
