import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailinboxsummaryComponent } from './crm-smm-gmailinboxsummary.component';

describe('CrmSmmGmailinboxsummaryComponent', () => {
  let component: CrmSmmGmailinboxsummaryComponent;
  let fixture: ComponentFixture<CrmSmmGmailinboxsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailinboxsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailinboxsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
