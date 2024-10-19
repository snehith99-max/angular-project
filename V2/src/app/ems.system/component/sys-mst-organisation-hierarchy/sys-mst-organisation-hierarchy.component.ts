import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-sys-mst-organisation-hierarchy',
  templateUrl: './sys-mst-organisation-hierarchy.component.html',
  styleUrls: ['./sys-mst-organisation-hierarchy.component.scss']
})

export class SysMstOrganisationHierarchyComponent {
  module_gid: any;
  responsedata: any;
  org_hierarchy_levellist: any;  

  constructor(private formBuilder: FormBuilder, private router: Router, private ActivatedRoute: ActivatedRoute, private socketservice: SocketService, private service: SocketService, private ToastrService: ToastrService ){}

  ngOnInit() {
    this.ActivatedRoute.queryParams.subscribe(params => {
      const urlparams = params['hash'];
      if (urlparams) {
        const decryptedParam = AES.decrypt(urlparams, environment.secretKey).toString(enc.Utf8);
        const paramvalues = decryptedParam.split('&');
        this.module_gid = paramvalues[0];
      }
    });

    let param = {
      module_gid : this.module_gid
    }
    
    var url = 'SysMstOriganisationHierarchy/GetOriganisationHierarchysummary'
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.org_hierarchy_levellist = this.responsedata.level_list;
    });
  }

  clear() {
    let param = {
      module_gid : this.module_gid
    }

    var api = 'SysMstOriganisationHierarchy/ClearHierarchy';
    this.service.post(api, param).subscribe((result: any) => {
      if(result.status == true) {
        this.ToastrService.success(result.message)
      }
      else {
        this.ToastrService.warning(result.message)
      }
    });
  }

  back() {
    this.router.navigate(['/system/SysMstModuleManager']);
  }

  // submit(module_gid : string) {
  //   let param = {
  //     module_gid : module_gid
  //   }

  //   var url = 'SysMstOriganisationHierarchy/GetOriganisationHierarchysummary'
  //   this.service.getparams(url, param).subscribe((result: any) => {
  //     this.responsedata = result;
  //     this.org_hierarchy_levellist = this.responsedata.level_list;
      
  //     setTimeout(() => {
  //       $('#org_hierarchy').DataTable();
  //     },);
  //   });
  // }
}
