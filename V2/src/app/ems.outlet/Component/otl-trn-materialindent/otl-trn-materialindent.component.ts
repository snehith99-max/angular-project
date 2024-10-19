  import { Component } from '@angular/core';
  import { SocketService } from 'src/app/ems.utilities/services/socket.service';
  import { Router } from '@angular/router';
  import { ToastrService } from 'ngx-toastr';
  import { NgxSpinnerService } from 'ngx-spinner';
  import { AES } from 'crypto-js';

  @Component({
    selector: 'app-otl-trn-materialindent',
    templateUrl: './otl-trn-materialindent.component.html',
    styleUrls: ['./otl-trn-materialindent.component.scss']
  })
  export class OtlTrnMaterialindentComponent {
  
    materialindent_list:any;
    responsedata: any;
    showOptionsDivId: any; 
    rows: any[] = [];
  
  
    constructor(public service :SocketService,private router:Router,private route:Router,private ToastrService: ToastrService,public NgxSpinnerService:NgxSpinnerService) {  
    }
    ngOnInit(): void {
      document.addEventListener('click', (event: MouseEvent) => {
        if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
          this.showOptionsDivId = null;
        }
      });
      this.GetIssueMaterialSummary();
    }
    GetIssueMaterialSummary(){
      var url = 'otltrnMI/MatrialIndentsummary'
      this.service.get(url).subscribe((result: any) => {
    
        this.responsedata = result;
        this.materialindent_list = this.responsedata.MIoutletsummary_list;
        setTimeout(() => {
          $('#materialindent_list').DataTable();
        }, 1);
      });
    }
    onadd(){
      this.router.navigate(['/outlet/OtlTrnRaisematerialindent']);
    }
    PrintPDF(materialrequisition_gid: any) {
      debugger;
      const api = 'ImsTrnMaterialIndent/GetmaterialindentRpt';
      this.NgxSpinnerService.show()
      let param = {
        materialrequisition_gid:materialrequisition_gid
      } 
      this.service.getparams(api,param).subscribe((result: any) => {
        if(result!=null){
          this.service.filedownload1(result);
        }
        this.NgxSpinnerService.hide()
      });
    }
  
    issue(params:any){
      debugger;
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.route.navigate(['/ims/ImsTrnMaterialindentIssue',encryptedParam]);
    }
    onview(params:any){
      const secretKey = 'storyboarderp';
      const param = (params);
      const encryptedParam = AES.encrypt(param,secretKey).toString();
      this.route.navigate(['/outlet/OtlTrnMaterialIndentView',encryptedParam]);
    }
    toggleOptions(account_gid: any) {
      if (this.showOptionsDivId === account_gid) {
        this.showOptionsDivId = null;
      } else {
        this.showOptionsDivId = account_gid;
      }
    }
  }





