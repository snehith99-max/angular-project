import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AES } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-hrm-mst-experiencelettersummary',
  templateUrl: './hrm-mst-experiencelettersummary.component.html',
  styles: [`
table thead th, 
.table tbody td { 
 position: relative; 
z-index: 0;
} 
.table thead th:last-child, 

.table tbody td:last-child { 
 position: sticky; 

right: 0; 
 z-index: 0; 

} 
.table td:last-child, 

.table th:last-child { 

padding-right: 50px; 

} 
.table.table-striped tbody tr:nth-child(odd) td:last-child { 

 background-color: #ffffff; 
  
  } 
  .table.table-striped tbody tr:nth-child(even) td:last-child { 
   background-color: #f2fafd; 

} 
`]
})

export class HrmMstExperiencelettersummaryComponent {
  responsedata: any;
  parameterValue: any;
  experience_gid: any;
  experience_list: any[] = [];

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private NgxSpinnerService: NgxSpinnerService, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
  }

  ngOnInit(): void {
    var url = 'HrmMstExperienceLetter/ExperienceLetterSummary'

    this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.experience_list = this.responsedata.Experiencesummary_list;
      setTimeout(() => {
        $('#experience_list').DataTable();
      },);
    });
  }

  PrintPDF(experience_gid: string) {
    debugger
    const api = 'HrmMstExperienceLetter/Getexperienceletterpdf';
    this.NgxSpinnerService.show()
    let param = {
      experience_gid: experience_gid,
    }
    this.service.getparams(api, param).subscribe((result: any) => {
      if (result != null) {
        this.service.filedownload1(result);
      }
      this.NgxSpinnerService.hide()
    });
  }
  ////////Delete popup////////
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }

  ondelete() {
    console.log(this.parameterValue);
    var url = 'HrmMstExperienceLetter/DeleteExperience'
    this.service.getid(url, this.parameterValue).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.ToastrService.success(result.message)
      }
    });
    setTimeout(function () {
      window.location.reload();
    }, 2000); // 2000 milliseconds = 2 seconds
  }
}