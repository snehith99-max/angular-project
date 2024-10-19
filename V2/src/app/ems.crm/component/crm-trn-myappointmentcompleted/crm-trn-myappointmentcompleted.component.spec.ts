import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentcompletedComponent } from './crm-trn-myappointmentcompleted.component';

describe('CrmTrnMyappointmentcompletedComponent', () => {
  let component: CrmTrnMyappointmentcompletedComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentcompletedComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentcompletedComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentcompletedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
