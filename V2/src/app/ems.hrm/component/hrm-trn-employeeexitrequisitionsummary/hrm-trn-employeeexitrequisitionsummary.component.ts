import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-hrm-trn-employeeexitrequisitionsummary',
  templateUrl: './hrm-trn-employeeexitrequisitionsummary.component.html',
  styleUrls: ['./hrm-trn-employeeexitrequisitionsummary.component.scss']
})
export class HrmTrnEmployeeexitrequisitionsummaryComponent {
  responsedata: any;
  exitrequisition_list: any[] = [];

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute, private router: Router, private ToastrService: ToastrService, public service: SocketService) {
   
    }

   ngOnInit(): void {
  
   //// Summary Grid//////
   debugger;
      
      var url = 'HrmtrnExitRequisition/GetExitRequisitionSummary'
      this.service.get(url).subscribe((result: any) => {
  
        this.responsedata = result;
        this.exitrequisition_list = this.responsedata.exitrequisition_list;
        setTimeout(() => {
          $('#exitrequisition_list').DataTable();
        }, );
  
  
      });
    }

  addexitrequisition() {
    this.router.navigate(['/hrm/HrmTrnEmployeeexitrequisitionadd'])
  }
}
