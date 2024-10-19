import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnAppointmentmanagementComponent } from './crm-trn-appointmentmanagement.component';

describe('CrmTrnAppointmentmanagementComponent', () => {
  let component: CrmTrnAppointmentmanagementComponent;
  let fixture: ComponentFixture<CrmTrnAppointmentmanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnAppointmentmanagementComponent]
    });
    fixture = TestBed.createComponent(CrmTrnAppointmentmanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
