import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES ,enc} from 'crypto-js';

@Component({
  selector: 'app-sbc-mst-dynamicdbexceptionerrorview',
  templateUrl: './sbc-mst-dynamicdbexceptionerrorview.component.html',
  styleUrls: ['./sbc-mst-dynamicdbexceptionerrorview.component.scss']
})
export class SbcMstDynamicdbexceptionerrorviewComponent {
  dynamicdbscript_list:any;
  responsedata:any;
employee:any;
  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService) {

  }
  ngOnInit(): void {
    debugger
    const dynamicdbscriptmanagement_gid = this.route.snapshot.paramMap.get('dynamicdbscriptmanagement_gid');
    // console.log(termsconditions_gid)
    this.employee = dynamicdbscriptmanagement_gid;
 
    const secretKey = 'storyboarderp';
 
    const deencryptedParam = AES.decrypt(this.employee, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetDynamicDBScriptViewsummary(deencryptedParam);
 
  }
 
  GetDynamicDBScriptViewsummary(dynamicdbscriptmanagement_gid: any) {
    debugger
    var url = 'Dynamicdb/GetDynamicDBScriptViewSummary'
    debugger
    let param = {
      dynamicdbscriptmanagement_gid: dynamicdbscriptmanagement_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.dynamicdbscript_list = result.serverlists;
    });
  }
}
