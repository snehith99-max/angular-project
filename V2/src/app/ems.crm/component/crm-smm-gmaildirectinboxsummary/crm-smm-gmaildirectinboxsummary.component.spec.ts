import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmaildirectinboxsummaryComponent } from './crm-smm-gmaildirectinboxsummary.component';

describe('CrmSmmGmaildirectinboxsummaryComponent', () => {
  let component: CrmSmmGmaildirectinboxsummaryComponent;
  let fixture: ComponentFixture<CrmSmmGmaildirectinboxsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmaildirectinboxsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmaildirectinboxsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
