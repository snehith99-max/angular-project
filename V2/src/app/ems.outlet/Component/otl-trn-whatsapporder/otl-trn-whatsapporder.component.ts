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
interface Iorderreject {
  kot_gid: string;
  reject_reason: string;
  customer_phone: string;
  contact_id: string;
  branch_gid: string;
  order_id: string;
}
interface Iorderupdate {
  kot_gid: string;
  order_update: string;
  customer_phone: string;
  contact_id: string;
  branch_gid: string;
  order_id: string;
}
@Component({
  selector: 'app-otl-trn-whatsapporder',
  templateUrl: './otl-trn-whatsapporder.component.html',
})
export class OtlTrnWhatsapporderComponent {
  responsedata: any;
  response_data: any;
  whatsappordersummary_list: any;
  showOptionsDivId: any;
  whatsappcodordersummary_list: any;
  orderrejectForm!: FormGroup;
  orderreject!: Iorderreject;
  orderupdateForm!: FormGroup;
  orderupdate!: Iorderupdate;
  parameterValue: any;
  constructor(private fb: FormBuilder, private NgxSpinnerService: NgxSpinnerService, private http: HttpClient, private route: ActivatedRoute, private router: Router, private service: SocketService, private ToastrService: ToastrService,) {
    this.orderreject = {} as Iorderreject;
    this.orderupdate = {} as Iorderupdate;


  }

  ngOnInit(): void {
    this.Getonlineordersum();
    this.orderrejectForm = new FormGroup({
      kot_gid: new FormControl(this.orderreject.kot_gid, [
      ]),
      contact_id: new FormControl(this.orderreject.contact_id, [
      ]),
      customer_phone: new FormControl(this.orderreject.customer_phone, [
      ]),
      order_id: new FormControl(this.orderreject.order_id, [
      ]),
      branch_gid: new FormControl(this.orderreject.branch_gid, [
      ]),
      reject_reason: new FormControl(this.orderreject.reject_reason, [
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/)
      ])
    });
    this.orderupdateForm = new FormGroup({
      kot_gid: new FormControl(this.orderupdate.kot_gid, [
      ]),
      contact_id: new FormControl(this.orderupdate.contact_id, [
      ]),
      customer_phone: new FormControl(this.orderupdate.customer_phone, [
      ]),
      order_id: new FormControl(this.orderupdate.order_id, [
      ]),
      branch_gid: new FormControl(this.orderupdate.branch_gid, [
      ]),
      order_update: new FormControl(this.orderupdate.order_update, [
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/)
      ])
    });
  }

  Getonlineordersum() {
    this.NgxSpinnerService.show();
    var api = 'OtlWhatsAppOrder/Getwhatsappordersummary';
    this.service.get(api).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      $('#whatsappordersummary_list').DataTable().destroy();
      this.response_data = result;
      this.whatsappordersummary_list = this.response_data.whatsappordersummary_list;
      setTimeout(() => {
        $('#whatsappordersummary_list').DataTable();
      }, 1);
    });


  }

  toggleOptions(kot_gid: any) {
    if (this.showOptionsDivId === kot_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = kot_gid;
    }
  }
  onview(params: any) {
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/outlet/OtlTrnWhatsapporderview', encryptedParam])
  }
  onpaid(kot_gid: any) {
    this.NgxSpinnerService.show();
    var api = 'OtlWhatsAppOrder/Updatewtsorderpayment';
    let param = {
      kot_gid: kot_gid
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
      }
      else {

        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.Getonlineordersum();
      }
    });
  }
  openModalreject(parameter: string) {
    this.parameterValue = parameter
    this.orderrejectForm.get("kot_gid")?.setValue(this.parameterValue.kot_gid);
    this.orderrejectForm.get("contact_id")?.setValue(this.parameterValue.contact_id);
    this.orderrejectForm.get("branch_gid")?.setValue(this.parameterValue.branch_gid);
    this.orderrejectForm.get("customer_phone")?.setValue(this.parameterValue.customer_phone);
    this.orderrejectForm.get("order_id")?.setValue(this.parameterValue.order_id);
    this.orderrejectForm.get("reject_reason")?.setValue(this.parameterValue.reject_reason);

  }
  onsend() {
    var url = 'WhatsApporderSummary/Updateorderrejectreason';
    this.service.post(url, this.orderrejectForm.value).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.Getonlineordersum();

      } else {
        this.ToastrService.success(result.message);
        this.Getonlineordersum();

      }
    });
  }
  openModalupadate(parameter: string) {
    this.parameterValue = parameter
    this.orderupdateForm.get("kot_gid")?.setValue(this.parameterValue.kot_gid);
    this.orderupdateForm.get("contact_id")?.setValue(this.parameterValue.contact_id);
    this.orderupdateForm.get("branch_gid")?.setValue(this.parameterValue.branch_gid);
    this.orderupdateForm.get("customer_phone")?.setValue(this.parameterValue.customer_phone);
    this.orderupdateForm.get("order_id")?.setValue(this.parameterValue.order_id);

  }
  onorderupadate() {
    var url = 'WhatsApporderSummary/orderupdates';
    this.service.post(url, this.orderupdateForm.value).pipe().subscribe((result: any) => {
      this.responsedata = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message);
        this.orderupdateForm.reset();

      } else {
        this.ToastrService.success(result.message);
        this.orderupdateForm.reset();


      }
    });
  }

  completedorder(kot_gid:any){
    let param = {
      kot_gid: kot_gid
    }
    var url = 'OtlWhatsAppOrder/completeorder';
    this.service.getparams(url, param).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
        this.Getonlineordersum();
      }
    });
 
  }
}
