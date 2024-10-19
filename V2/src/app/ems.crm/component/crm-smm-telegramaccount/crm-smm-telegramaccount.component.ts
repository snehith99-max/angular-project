import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators ,  ValidationErrors,
  AbstractControl,
  ValidatorFn} from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface ITelegram {
  telegram_caption: string;
  file_path:string;
}
@Component({
  selector: 'app-crm-smm-telegramaccount',
  templateUrl: './crm-smm-telegramaccount.component.html',
  styleUrls: ['./crm-smm-telegramaccount.component.scss']
})
export class CrmSmmTelegramaccountComponent {
  file!: File;
  telegram_list: any;
  telegram!: ITelegram;
  TelegramForm!: FormGroup;
  invite_link: any;
  id: any;
  bot_name: any;
  group: any;
  username: any;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private route: Router,) {
    this.telegram = {} as ITelegram;
  }
  ngOnInit(): void {
    
    this.TelegramForm = new FormGroup({
      telegram_caption: new FormControl(this.telegram.telegram_caption, [
        Validators.required,
        this.noWhitespaceValidator(),

      ]),
      
      file_path: new FormControl(''),




    });
    var url = 'Telegram/GetTelegram'
    this.service.get(url).subscribe((result,) => {

      this.telegram_list = result;
      this.id =this.telegram_list.result.id;
      this.bot_name = this.telegram_list.result.username;
      this.group = this.telegram_list.result.title;
      this.invite_link = this.telegram_list.result.invite_link;

    });
  }
   onChange2(event:any) {
    this.file =event.target.files[0];
    }
     //////////// Form validtion////////
get telegram_caption() {
  return this.TelegramForm.get('telegram_caption')!;
}
public onsubmit(): void {

  let formData = new FormData();
  if(this.file !=null &&  this.file != undefined){

     
     formData.append("file", this.file,this.file.name);
   
     formData.append("telegram_caption", this.TelegramForm.value.telegram_caption);
     var url='Telegram/TelegramUpload'
     this.service.postfile(url,formData).subscribe((result:any) => {

       if(result.status ==false){

         this.ToastrService.warning(result.message)
          window.location.reload();
       }
       else{
       
         this.TelegramForm.reset();
         this.ToastrService.success(result.message)
         this.route.navigate(['/crm/CrmSmmTelegramsummary']);
       }
     });

  }
  else if(this.TelegramForm.value.telegram_caption !=null &&  this.TelegramForm.value.telegram_caption != undefined){
    var url = 'Telegram/Telegrammessage'

    this.service.post(url,this.TelegramForm.value).pipe().subscribe((result:any)=>{
      
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        window.location.reload();
      
        
      }
      else{
        this.TelegramForm.reset();
        this.ToastrService.success(result.message)
        this.route.navigate(['/crm/CrmSmmTelegramsummary']);
 
        
      }
    }); 
}

  else{
   this.ToastrService.warning("Kindly Upload Image/Video !!")
  }
}
noWhitespaceValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const isWhitespace = (control.value || '').trim().length === 0;
    return isWhitespace ? { whitespace: true } : null;
  };
}
}
