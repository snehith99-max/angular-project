import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmenttodayComponent } from './crm-trn-myappointmenttoday.component';

describe('CrmTrnMyappointmenttodayComponent', () => {
  let component: CrmTrnMyappointmenttodayComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmenttodayComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmenttodayComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmenttodayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
