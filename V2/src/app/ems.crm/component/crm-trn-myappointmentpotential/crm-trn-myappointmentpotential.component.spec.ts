import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentpotentialComponent } from './crm-trn-myappointmentpotential.component';

describe('CrmTrnMyappointmentpotentialComponent', () => {
  let component: CrmTrnMyappointmentpotentialComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentpotentialComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentpotentialComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentpotentialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
