import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlookcampaigntemplateComponent } from './crm-smm-outlookcampaigntemplate.component';

describe('CrmSmmOutlookcampaigntemplateComponent', () => {
  let component: CrmSmmOutlookcampaigntemplateComponent;
  let fixture: ComponentFixture<CrmSmmOutlookcampaigntemplateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlookcampaigntemplateComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlookcampaigntemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
