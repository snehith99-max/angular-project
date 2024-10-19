import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlookcampaignsentsummaryComponent } from './crm-smm-outlookcampaignsentsummary.component';

describe('CrmSmmOutlookcampaignsentsummaryComponent', () => {
  let component: CrmSmmOutlookcampaignsentsummaryComponent;
  let fixture: ComponentFixture<CrmSmmOutlookcampaignsentsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlookcampaignsentsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlookcampaignsentsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
