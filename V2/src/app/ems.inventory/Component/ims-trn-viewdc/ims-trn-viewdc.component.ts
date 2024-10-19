import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-ims-trn-viewdc',
  templateUrl: './ims-trn-viewdc.component.html',
  styleUrls: ['./ims-trn-viewdc.component.scss']
})
export class ImsTrnViewdcComponent {

  opendc:any;
  productsummary_list: any[]=[];
  viewDC_list : any[]=[];

  dc_date : any;
  directorder_gid:any;
  Branch_name : any;
  shipping_to : any;
  mode_of_despatch : any;
  no_of_boxs : any;
  tracker_id : any;
  dc_note : any;
  responsedata: any
  constructor(private formBuilder: FormBuilder,private route:Router,private router:ActivatedRoute,public service :SocketService) {
   
  }


  ngOnInit(): void {
    this.opendc= this.router.snapshot.paramMap.get('directorder_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.opendc,secretKey).toString(enc.Utf8);
    this.GetviewDC(deencryptedParam);
    this.GetViewDCProduct(deencryptedParam);
  }

  GetviewDC(directorder_gid:any){

    var url='ImsTrnOpenDcSummary/GetViewDCSummary'
    let param = {
      directorder_gid :directorder_gid
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
      this.viewDC_list = result.viewdc_list;
      this.dc_date=this.viewDC_list[0].directorder_date;
      this.directorder_gid = this.viewDC_list[0].directorder_gid;
      this.Branch_name = this.viewDC_list[0].branch_name;
      this.shipping_to = this.viewDC_list[0].shipping_to;
      this.mode_of_despatch = this.viewDC_list[0].mode_of_despatch;
      this.no_of_boxs = this.viewDC_list[0].no_of_boxs;
      this.tracker_id = this.viewDC_list[0].tracker_id;
      this.dc_note = this.viewDC_list[0].dc_note;
    })
  }
  GetViewDCProduct(directorder_gid:any){
    var api = 'ImsTrnOpenDcSummary/GetdcProduct';
    let param = {
      directorder_gid :directorder_gid
    }
    this.service.getparams(api,param).subscribe((result: any) => {
      this.responsedata = result;
      this.productsummary_list = result.tmpdcproduct_list;
    });
  }
  ondelete(){}
}
