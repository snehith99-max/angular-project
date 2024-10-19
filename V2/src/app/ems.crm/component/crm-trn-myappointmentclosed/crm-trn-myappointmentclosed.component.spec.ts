import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentclosedComponent } from './crm-trn-myappointmentclosed.component';

describe('CrmTrnMyappointmentclosedComponent', () => {
  let component: CrmTrnMyappointmentclosedComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentclosedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentclosedComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentclosedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
