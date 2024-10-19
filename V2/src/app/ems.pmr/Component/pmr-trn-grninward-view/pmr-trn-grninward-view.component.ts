import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-pmr-trn-grninward-view',
  templateUrl: './pmr-trn-grninward-view.component.html',
  styleUrls: ['./pmr-trn-grninward-view.component.scss']
})
export class PmrTrnGrninwardViewComponent {

  GrnInward_lists: any[] = [];
  GrnEditInward_lists: any[] = [];
  reactiveForm!: FormGroup;
  vendor: any;
  responsedata: any;
  grn_gid: any;
  GetEditGrnInward_lists: any;
  GetEditGrnInwardproduct_lists: any;
  grninward: any;
  GetViewGrnInward_lists: any;
  grngid: any;
  lspage:any;


  constructor(private router:ActivatedRoute,public service :SocketService,public NgxSpinnerService:NgxSpinnerService,private route:Router  ) { 

  }

  ngOnInit(): void {
    

debugger

    this.grninward= this.router.snapshot.paramMap.get('grn_gid');
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.grninward,secretKey).toString(enc.Utf8);
    this.lspage = this.router.snapshot.paramMap.get('lspage');
    this.lspage = this.lspage;
    const lspage = AES.decrypt(this.lspage, secretKey).toString(enc.Utf8);
    this.lspage = lspage;
    console.log(deencryptedParam)
    this.GetEditGrnInward(deencryptedParam);
    this.GetEditGrnInwardproduct(deencryptedParam);    
  }
  GetEditGrnInward(grn_gid: any) {
    this.NgxSpinnerService.show();
    var url='PmrTrnGrnInward/GetEditGrnInward'
    let param = {
      grn_gid : grn_gid
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.  GetEditGrnInward_lists = result.GetEditGrnInward_lists;
    //console.log(this.employeeedit_list)
    this.NgxSpinnerService.hide();

  });

  }
  GetEditGrnInwardproduct(grn_gid: any) {
    var url='PmrTrnGrninward/GetEditGrnInwardproduct'
    let param = {
      grn_gid : grn_gid
    }
    this.service.getparams(url,param).subscribe((result:any)=>{
    this.GetEditGrnInwardproduct_lists = result.GetEditGrnInwardproduct_lists;
    //console.log(this.employeeedit_list)

  });

  }

    back() {
      if(this.lspage == 'Inventory'){
        this.route.navigate(['/ims/ImsRptGRNReport']);
      }
      else{
        this.route.navigate(['/pmr/PmrTrnGrninward']);
      }
  
  }
}
