import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
@Component({
  selector: 'app-tsk-mst-deployment-tracker',
  templateUrl: './tsk-mst-deployment-tracker.component.html',
  styleUrls: ['./tsk-mst-deployment-tracker.component.scss']
})
export class TskMstDeploymentTrackerComponent {
  deploy_list:any
  deploy_module: any;
  constructor(public router: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService) {
  }
  ngOnInit() {
    this.NgxSpinnerService.show();
    var url = 'TskTrnTaskManagement/deploysummary';
    this.SocketService.get(url).subscribe((result: any) => {
      if (result.depmodule_summary != null) {
        $('#deploy').DataTable().destroy();
        this.deploy_list = result.depmodule_summary;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#deploy').DataTable();
        }, 1);
      }
      else {
        this.deploy_list = result.depmodule_summary;
        setTimeout(() => {
          var table = $('#deploy').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#deploy').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
      this.NgxSpinnerService.hide();
    });
  }
  view(params:any){debugger
    const parameter1 = `${params}&Pending`;
    const secretKey = 'storyboarderp';
    const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
      var url = '/ITS/IskMstDeployView?hash=' + encodeURIComponent (encryptedParam);
    this.router.navigateByUrl(url)
  }
  Edit(params:any){debugger
    const parameter1 = `${params}&Pending`;
    const secretKey = 'storyboarderp';
    const encryptedParam = AES.encrypt(parameter1,secretKey).toString();
      var url = '/ITS/IskMstDeployEdit?hash=' + encodeURIComponent (encryptedParam);
    this.router.navigateByUrl(url)
  }
  Module(deployment_trackergid:any){
    var params = {
      deployment_trackergid: deployment_trackergid,
    }; 
    this.NgxSpinnerService.show()
    var url = 'TskTrnTaskManagement/deployview';
    this.SocketService.getparams(url,params).subscribe((result: any) => { 
      this.deploy_module=result.deploy_module

      this.NgxSpinnerService.hide();
    });
  }
}
