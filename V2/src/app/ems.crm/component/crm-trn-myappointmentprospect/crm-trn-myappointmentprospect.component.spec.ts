import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyappointmentprospectComponent } from './crm-trn-myappointmentprospect.component';

describe('CrmTrnMyappointmentprospectComponent', () => {
  let component: CrmTrnMyappointmentprospectComponent;
  let fixture: ComponentFixture<CrmTrnMyappointmentprospectComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyappointmentprospectComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyappointmentprospectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
