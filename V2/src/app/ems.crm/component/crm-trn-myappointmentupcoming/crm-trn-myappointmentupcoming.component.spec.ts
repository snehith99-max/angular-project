import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentupcomingComponent } from './crm-trn-myappointmentupcoming.component';

describe('CrmTrnMyappointmentupcomingComponent', () => {
  let component: CrmTrnMyappointmentupcomingComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentupcomingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentupcomingComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentupcomingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
