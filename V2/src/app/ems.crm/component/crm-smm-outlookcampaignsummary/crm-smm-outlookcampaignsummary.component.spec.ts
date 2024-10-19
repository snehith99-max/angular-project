import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlookcampaignsummaryComponent } from './crm-smm-outlookcampaignsummary.component';

describe('CrmSmmOutlookcampaignsummaryComponent', () => {
  let component: CrmSmmOutlookcampaignsummaryComponent;
  let fixture: ComponentFixture<CrmSmmOutlookcampaignsummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlookcampaignsummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlookcampaignsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
