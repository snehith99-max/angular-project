import { Component, ElementRef, Renderer2 } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { AES, enc } from 'crypto-js';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-smr-mst-sequencecodeedit',
  templateUrl: './smr-mst-sequencecodeedit.component.html',
  styleUrls: ['./smr-mst-sequencecodeedit.component.scss']
})
export class SmrMstSequencecodeeditComponent {

  sequencecustomizer_gid : any;
  sequencecodeform : FormGroup | any;
  responsedata : any;
  sequence_list:any[]=[];
  sequence_name : any;
  sequence_code : any;


  constructor(private renderer: Renderer2, private el: ElementRef,public service :SocketService, public NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService,private route:Router,private router: ActivatedRoute ) {
    this.sequencecodeform = new FormGroup({
      company_code : new FormControl(''),
      runningno_prefix : new FormControl(''),
      sequence_curval : new FormControl(''),
      delimiter : new FormControl(''),
      month_flag :new FormControl(''),
      year_flag:new FormControl(''),
      location_flag:new FormControl(''),
      department_flag:new FormControl(''),
      businessunit_flag:new FormControl(''),
      branchflag:new FormControl(''),
      active_flag:new FormControl(''),
      active_flag1:new FormControl(''),
      active_flag2:new FormControl(''),
      active_flag3:new FormControl(''),
      active_flag4:new FormControl(''),
      active_flag5:new FormControl(''),
    })
  }
  ngOnInit() : void {
    debugger
    const sequencecustomizer_gid = this.router.snapshot.paramMap.get('sequencecodecustomizer_gid');
    this.sequencecustomizer_gid = sequencecustomizer_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.sequencecustomizer_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.sequencecustomizer_gid = deencryptedParam
    this.GetSeqCodeEdit(deencryptedParam)

    this.sequencecodeform = new FormGroup({
      company_code : new FormControl(''),
      runningno_prefix : new FormControl(''),
      sequence_curval : new FormControl(''),
      delimiter : new FormControl(''),
      month_flag :new FormControl(''),
      year_flag:new FormControl(''),
      location_flag:new FormControl(''),
      department_flag:new FormControl(''),
      businessunit_flag:new FormControl(''),
      branchflag:new FormControl(''),
      active_flag:new FormControl(''),
      active_flag1:new FormControl(''),
      active_flag2:new FormControl(''),
      active_flag3:new FormControl(''),
      active_flag4:new FormControl(''),
      active_flag5:new FormControl(''),
    })
  }
  GetSeqCodeEdit(deencryptedParam : any){

    var url = 'SmrMstSequenceCodeCustomizer/GetSequenceCodeCustomizerEdit'
    let param ={
      sequencecodecustomizer_gid : deencryptedParam
    }
    this.service.getparams(url,param).subscribe((result: any) => {
      this.responsedata=result;
      this.sequence_list = result.SequenceCodeSummary;
      this.sequencecodeform.get("company_code")?.setValue(this.sequence_list[0].company_code);
      this.sequencecodeform.get("runningno_prefix")?.setValue(this.sequence_list[0].runningno_prefix);
      this.sequencecodeform.get("sequence_curval")?.setValue(this.sequence_list[0].sequence_curval);
      this.sequencecodeform.get("delimiter")?.setValue(this.sequence_list[0].delimeter);
      this.sequencecodeform.get("active_flag")?.setValue(this.sequence_list[0].branch_flag);
      this.sequencecodeform.get("active_flag1")?.setValue(this.sequence_list[0].businessunit_flag);
      this.sequencecodeform.get("active_flag2")?.setValue(this.sequence_list[0].department_flag);
      this.sequencecodeform.get("active_flag3")?.setValue(this.sequence_list[0].location_flag);
      this.sequencecodeform.get("active_flag4")?.setValue(this.sequence_list[0].year_flag);
      this.sequencecodeform.get("active_flag5")?.setValue(this.sequence_list[0].month_flag);
      this.sequence_code = this.sequence_list[0].sequence_code
      this.sequence_name = this.sequence_list[0].sequence_name
      
     
    });
  }
  submit(){
    var url = 'SmrMstSequenceCodeCustomizer/GetSequenceCodeCustomizerUpdate'
    let param ={
      sequencecodecustomizer_gid : this.sequencecustomizer_gid,
      company_code : this.sequencecodeform.value.company_code,
      runningno_prefix : this.sequencecodeform.value.runningno_prefix,
      sequence_curval: this.sequencecodeform.value.sequence_curval,
      month_flag: this.sequencecodeform.value.active_flag5,
      delimeter: this.sequencecodeform.value.delimiter,
      year_flag: this.sequencecodeform.value.active_flag4,
      location_flag: this.sequencecodeform.value.active_flag3,
      department_flag: this.sequencecodeform.value.active_flag2,
      businessunit_flag: this.sequencecodeform.value.active_flag1,
      branch_flag: this.sequencecodeform.value.active_flag,
    }
    this.service.post(url,param).pipe().subscribe((result:any)=>{
      this.responsedata=result;
      if(result.status ==false){
        this.ToastrService.warning(result.message)
        this.route.navigate(['/smr/SmrMstSequencecustomizer']);
        this.NgxSpinnerService.hide();
       
      }
      else{
        this.ToastrService.success(result.message)
        this.route.navigate(['/smr/SmrMstSequencecustomizer']);
        this.NgxSpinnerService.hide(); 
      }
     
  });
  }
}
