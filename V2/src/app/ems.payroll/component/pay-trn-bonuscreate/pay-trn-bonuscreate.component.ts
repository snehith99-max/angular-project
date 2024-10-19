import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl,FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-pay-trn-bonuscreate',
  templateUrl: './pay-trn-bonuscreate.component.html',
  styleUrls: ['./pay-trn-bonuscreate.component.scss']
})
export class PayTrnBonuscreateComponent {
  reactiveForm!: FormGroup;
  response_data : any;
  parameterValue: any;



  ngOnInit(): void {
    const options: Options = {
      dateFormat: 'd-m-Y',    
    };
    flatpickr('.date-picker', options);
    }


    constructor(public service :SocketService,private route:Router,private ToastrService: ToastrService, private FormBuilder: FormBuilder) {
      this.reactiveForm = new FormGroup({
        bonus_name: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
        bonus_date: new FormControl('', [Validators.required]),
        bonus_todate: new FormControl('', [Validators.required]),
        bonus_percentage: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
        remarks: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      });
    }
  
    get bonus_name() {
      return this.reactiveForm.get('entityname')!;
    }  
    get bonus_percentage() {
      return this.reactiveForm.get('entityname')!;
    }
    get remarks() {
      return this.reactiveForm.get('entityname')!;
    }
    get bonus_date() {
      return this.reactiveForm.get('bonus_date')!;
    }
    
    get bonus_todate() {
      return this.reactiveForm.get('bonus_todate')!;
    }

    get todateControl() {
      return this.reactiveForm.get('bonus_todate');
      }

  submit() {
    const api = 'PayTrnBonus/PostBonus'
    this.service.post(api, this.reactiveForm.value).subscribe((result: any) => {
      this.response_data = result;
      if (result.status == false) {
        this.ToastrService.warning(result.message)
      }
      else {
        this.route.navigate(['/payroll/PayTrnBonus']);
        this.ToastrService.success(result.message)
      }
         
    });
   
  }

  onKeyPress(event: any) {
    // Get the pressed key
    const key = event.key;

    // Check if the key is a number or a dot (for decimal point)
    if (!/^[0-9.]$/.test(key)) {
      // If not a number or dot, prevent the default action (key input)
      event.preventDefault();
    }
  }


 
}
