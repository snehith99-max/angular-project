import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelemycampaignDropComponent } from './crm-trn-telemycampaign-drop.component';

describe('CrmTrnTelemycampaignDropComponent', () => {
  let component: CrmTrnTelemycampaignDropComponent;
  let fixture: ComponentFixture<CrmTrnTelemycampaignDropComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelemycampaignDropComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelemycampaignDropComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
