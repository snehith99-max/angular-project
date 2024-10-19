import { Component, ElementRef, HostListener, Input , EventEmitter, Output } from '@angular/core';

import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
interface INotes {
  notes_detail: string;
  s_no: string;

}
@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.scss']
})
export class NotesComponent {
  notesopen: boolean = true;
  reactiveForm!: FormGroup;
  notes!: INotes;
  responsedata: any;
  notesupdatelist: any[] = [];
  s_no: any;
  searchText = '';

  notes_detailedit: any;
  matchesSearch(item: any): boolean {
    const searchString = this.searchText.toLowerCase();
    return item.notes_detail.toLowerCase().includes(searchString) || item.notes_detail.toLowerCase().includes(searchString);
  }
  constructor(private _eref: ElementRef, private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) {
    this.notes = {} as INotes;

  }

  @Output() iconClicked: EventEmitter<void> = new EventEmitter<void>();


  data = [
    { data_fst: 'textarea data 1' }, { data_fst: 'textarea data 2' }, { data_fst: 'textarea data 3' }, { data_fst: 'textarea data 4' }
  ]
  ngOnInit(): void {
    this.GetnotesSummary();
    this.reactiveForm = new FormGroup({
      notes_detail: new FormControl(this.notes.notes_detail, [
        Validators.required,
      ]),
    });


  }
  get notes_detail() {
    return this.reactiveForm.get('notes_detail')!;
  }
  public onadd(): void {
    if (this.reactiveForm.value.notes_detail != null) {

      this.reactiveForm.value;
      var url = 'Features/Postnotesupdate'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();
          this.GetnotesSummary();
        }
        else {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();
          this.GetnotesSummary();

        }
      });
    }
    else {
      this.ToastrService.warning('Notes Value Is Required!!')
    }
  }
  public onupdate(i: any): void {
    if (this.notesupdatelist[i].notes_detail != "" && this.notesupdatelist[i].notes_detail != undefined) {
      let param = {
        s_no: this.notesupdatelist[i].s_no,
        notes_detail: this.notesupdatelist[i].notes_detail
      };
      var url = 'Features/Updatednotes';
      this.service.post(url, param).subscribe((result: any) => {
        if (result.status === false) {
          this.ToastrService.warning(result.message)
        } else {
          this.ToastrService.success(result.message)
        }
        this.GetnotesSummary();
      });
    }
    else {
      this.ToastrService.warning('Notes Value Is Required!! ')

    }
  }

  ondelete(i: any) {
    var url = 'Features/deletenotes'
    let param = {
      s_no: this.notesupdatelist[i].s_no,
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status === false) {
        this.ToastrService.warning(result.message)
      } else {
        this.ToastrService.success(result.message)
      }
      this.GetnotesSummary();

    });
  }
  GetnotesSummary() {
    var url = 'Features/GetnotesSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#productgroup_list').DataTable().destroy();
      this.responsedata = result;
      this.notesupdatelist = this.responsedata.notesupdate_list;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#productgroup_list').DataTable();
      }, 1);


    });


  }
  determineColor(index: number): string {
    const colors = ['blue', 'green', 'red', 'yellow'];
    return colors[index % colors.length];
  }

  notesOpenfunction() {
    this.notesopen = !this.notesopen
  }


  onClick(): void {
    this.iconClicked.emit();
  }

  @HostListener('document:click', ['$event'])
  clickout(event: Event) {
    if (!this._eref.nativeElement.contains(event.target)) {
      this.notesopen = true;
    }
  }
}


