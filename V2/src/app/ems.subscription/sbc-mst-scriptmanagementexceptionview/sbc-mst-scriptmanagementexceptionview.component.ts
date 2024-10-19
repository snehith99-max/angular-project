import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES ,enc} from 'crypto-js';
@Component({
  selector: 'app-sbc-mst-scriptmanagementexceptionview',
  templateUrl: './sbc-mst-scriptmanagementexceptionview.component.html',
  styleUrls: ['./sbc-mst-scriptmanagementexceptionview.component.scss']
})
export class SbcMstScriptmanagementexceptionviewComponent {
  scripts_list:any;
  employee:any;

  constructor(private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService) {

  }
  ngOnInit(): void {
    debugger
    const dbscriptmanagementdocument_gid = this.route.snapshot.paramMap.get('dbscriptmanagementdocument_gid');
    // console.log(termsconditions_gid)
    this.employee = dbscriptmanagementdocument_gid;
 
    const secretKey = 'storyboarderp';
 
    const deencryptedParam = AES.decrypt(this.employee, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetScriptViewsummary(deencryptedParam);
 
  }
 
  GetScriptViewsummary(dbscriptmanagementdocument_gid: any) {
    debugger
    var url = 'Scriptmanagement/GetScriptexceptionViewSummary'
    debugger
    let param = {
      dbscriptmanagementdocument_gid: dbscriptmanagementdocument_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.scripts_list = result.serverlists;
    });
  }
}
