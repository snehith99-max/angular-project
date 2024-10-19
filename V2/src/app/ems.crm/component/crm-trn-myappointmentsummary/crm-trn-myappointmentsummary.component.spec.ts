import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentsummaryComponent } from './crm-trn-myappointmentsummary.component';

describe('CrmTrnMyappointmentsummaryComponent', () => {
  let component: CrmTrnMyappointmentsummaryComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
