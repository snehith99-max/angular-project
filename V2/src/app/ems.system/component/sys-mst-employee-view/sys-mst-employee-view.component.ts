import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';

@Component({
  selector: 'app-sys-mst-employee-view',
  templateUrl: './sys-mst-employee-view.component.html',
  styleUrls: ['./sys-mst-employee-view.component.scss']
})

export class SysMstEmployeeViewComponent {
  employee: any;
  employeeedit_list: any;
   employeeprofile:any;

  constructor(private formBuilder: FormBuilder, private route: Router, private router: ActivatedRoute, public service: SocketService) { }

  ngOnInit(): void {
    const employee_gid = this.router.snapshot.paramMap.get('employee_gid');
    this.employee = employee_gid;
    const secretKey = 'storyboarderp';

    const deencryptedParam = AES.decrypt(this.employee, secretKey).toString(enc.Utf8);
    this.GetEditEmployeeSummary(deencryptedParam);
  }
  
  GetEditEmployeeSummary(employee_gid: any) {
    var url = 'Employeelist/GetEditEmployeeSummary'
    let param = {
      employee_gid: employee_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.employeeedit_list = result.GetEditEmployeeSummary;
    });
  }

  // ViewUploadphoto(employee_photo: any, photo_name: any) {
  //   photo_name = "employee_profile";
  //   const image = employee_photo.split('.net/');
  //   const page = image[0];
  //   const url = page.split('?');
  //   const imageurl = url[0];
  //   const parts = imageurl.split('.');
  //   const extension = parts.pop();

  //   this.service.downloadfile(imageurl, photo_name + '.' + extension).subscribe(
  //     (data: any) => {
  //       if (data != null) {
  //       }
  //     });
  // }
  ViewUploadphoto(employee_photo:any,photo_name:any){
    debugger
       photo_name = "employee_profile";
        const image = employee_photo.split('.net/');
      const page = image[0];
     const url = page.split('?');
      const imageurl = url[0];
      const parts = imageurl.split('.');
      const extension = parts.pop();
    
      this.service.downloadfile(imageurl, photo_name+'.'+ extension).subscribe(
        (data: any) => {
        if (data != null){
          this.service.fileviewer(data);

        }
      
      });
      }
}