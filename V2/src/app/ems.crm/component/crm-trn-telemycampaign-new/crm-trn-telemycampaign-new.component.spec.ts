import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTelemycampaignNewComponent } from './crm-trn-telemycampaign-new.component';

describe('CrmTrnTelemycampaignNewComponent', () => {
  let component: CrmTrnTelemycampaignNewComponent;
  let fixture: ComponentFixture<CrmTrnTelemycampaignNewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTelemycampaignNewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTelemycampaignNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
