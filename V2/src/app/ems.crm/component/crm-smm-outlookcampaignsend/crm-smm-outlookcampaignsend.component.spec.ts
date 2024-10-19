import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlookcampaignsendComponent } from './crm-smm-outlookcampaignsend.component';

describe('CrmSmmOutlookcampaignsendComponent', () => {
  let component: CrmSmmOutlookcampaignsendComponent;
  let fixture: ComponentFixture<CrmSmmOutlookcampaignsendComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlookcampaignsendComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlookcampaignsendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
