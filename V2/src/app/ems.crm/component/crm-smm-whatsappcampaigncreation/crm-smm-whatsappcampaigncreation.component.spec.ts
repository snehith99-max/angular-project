import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmWhatsappcampaigncreationComponent } from './crm-smm-whatsappcampaigncreation.component';

describe('CrmSmmWhatsappcampaigncreationComponent', () => {
  let component: CrmSmmWhatsappcampaigncreationComponent;
  let fixture: ComponentFixture<CrmSmmWhatsappcampaigncreationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmWhatsappcampaigncreationComponent]
    });
    fixture = TestBed.createComponent(CrmSmmWhatsappcampaigncreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
