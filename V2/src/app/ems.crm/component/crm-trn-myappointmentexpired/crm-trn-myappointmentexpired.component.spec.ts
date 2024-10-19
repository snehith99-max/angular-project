import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentexpiredComponent } from './crm-trn-myappointmentexpired.component';

describe('CrmTrnMyappointmentexpiredComponent', () => {
  let component: CrmTrnMyappointmentexpiredComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentexpiredComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentexpiredComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentexpiredComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
