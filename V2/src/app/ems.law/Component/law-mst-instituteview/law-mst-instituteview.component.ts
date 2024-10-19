import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Location } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-law-mst-instituteview',
  templateUrl: './law-mst-instituteview.component.html',
  styleUrls: ['./law-mst-instituteview.component.scss']
})
export class LawMstInstituteviewComponent implements OnInit{
  institutegid: any;
  InstituteList: any;
  constructor(private NgxSpinnerService:NgxSpinnerService,
    private router:ActivatedRoute,
    public service :SocketService,
    private location: Location,
    private SocketService: SocketService,) {}

  backbutton(){
    this.location.back();
  }

  ngOnInit(): void 
  {
    const institute_gid =this.router.snapshot.paramMap.get('institute_gid');
    this.institutegid= institute_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.institutegid,secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.GetInstituteEditSummary(deencryptedParam);  
} 
GetInstituteEditSummary(institute_gid: any) {
  debugger
  var url = 'LawMstInstitute/GetInstituteEditSummary'
    let param = {
      institute_gid : institute_gid 
    }
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      this.InstituteList = result.institute_List;
  this.NgxSpinnerService.hide();
  });
}
}