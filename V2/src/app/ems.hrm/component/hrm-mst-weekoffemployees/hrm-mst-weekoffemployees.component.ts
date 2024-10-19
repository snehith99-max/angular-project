import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup,Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from '../../../ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES, enc } from 'crypto-js';

@Component({
  selector: 'app-hrm-mst-weekoffemployees',
  templateUrl: './hrm-mst-weekoffemployees.component.html',
  styleUrls: ['./hrm-mst-weekoffemployees.component.scss']
})
export class HrmMstWeekoffemployeesComponent {

  Weekoffform:FormGroup;
  employee_gid1:any[] =[];
  mdlMondayName:any;  
  mdltuesdayName:any;
  mdlwednesdayName:any;
  mdlthursdayName:any;
  mdlfridayName:any;
  mdlsaturdayName:any;
  mdlsundayName:any;
  employee_name:any;
  decryptedArray:any;
  weekoff_list: any[] = [];
  constructor(private SocketService: SocketService,private route: Router, private router: ActivatedRoute,private NgxSpinnerService: NgxSpinnerService,public service: SocketService,private ToastrService: ToastrService,private FormBuilder: FormBuilder) {

    this.Weekoffform = new FormGroup({
      monday:  new FormControl('',[Validators.required]),
      employee_gid:  new FormControl(''),
      tuesday:  new FormControl('',[Validators.required]),
      wednesday: new FormControl('',[Validators.required]),
      thursday: new FormControl('',[Validators.required]),
      friday: new FormControl('',[Validators.required]),
      saturday: new FormControl('',[Validators.required]),
      sunday: new FormControl('',[Validators.required]),
    });
  }

  ngOnInit(){
    debugger
    const encryptedParam = this.router.snapshot.paramMap.get('weekoff_list1');
     const secretKey = 'storyboarderp';
     if (!encryptedParam) {
      console.error('Parameter not found in the route.');
      return;
  }
  const decryptedString = AES.decrypt(encryptedParam, secretKey).toString(enc.Utf8);
  this.decryptedArray = JSON.parse(decryptedString);
}
backbutton(){
  this.route.navigate(['/hrm/HrmMstWeekoffmanagement'])
  this.Weekoffform.reset();
}
updateweekoffemployee(){
  debugger
  
  
  var url="WeekOff/updateweekoffemployee";
  var params ={
    employee_gid1: this.decryptedArray,
    monday : this.Weekoffform.value.monday,
    tuesday : this.Weekoffform.value.tuesday,
    wednesday : this.Weekoffform.value.wednesday,
    thursday : this.Weekoffform.value.thursday,
    friday : this.Weekoffform.value.friday,
    saturday : this.Weekoffform.value.saturday,
    sunday : this.Weekoffform.value.sunday,
  }
  this.SocketService.postparams(url,params).subscribe((result:any)=>{
    if(result.status==true){
      this.ToastrService.success(result.message)
      this.route.navigate(['/hrm/HrmMstWeekoffmanagement'])
    }
    else{
      this.ToastrService.warning(result.message)
    }

  });

}
     
  }

