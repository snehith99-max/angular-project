import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentdropComponent } from './crm-trn-myappointmentdrop.component';

describe('CrmTrnMyappointmentdropComponent', () => {
  let component: CrmTrnMyappointmentdropComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentdropComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentdropComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentdropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
