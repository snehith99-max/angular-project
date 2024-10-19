import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlookinboxsummaryComponent } from './crm-smm-outlookinboxsummary.component';

describe('CrmSmmOutlookinboxsummaryComponent', () => {
  let component: CrmSmmOutlookinboxsummaryComponent;
  let fixture: ComponentFixture<CrmSmmOutlookinboxsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlookinboxsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlookinboxsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
