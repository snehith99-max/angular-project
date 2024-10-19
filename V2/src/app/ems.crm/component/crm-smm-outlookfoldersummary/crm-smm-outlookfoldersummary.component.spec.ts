import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlookfoldersummaryComponent } from './crm-smm-outlookfoldersummary.component';

describe('CrmSmmOutlookfoldersummaryComponent', () => {
  let component: CrmSmmOutlookfoldersummaryComponent;
  let fixture: ComponentFixture<CrmSmmOutlookfoldersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlookfoldersummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlookfoldersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
