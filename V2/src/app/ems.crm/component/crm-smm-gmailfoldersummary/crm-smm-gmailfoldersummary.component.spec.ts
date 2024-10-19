import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailfoldersummaryComponent } from './crm-smm-gmailfoldersummary.component';

describe('CrmSmmGmailfoldersummaryComponent', () => {
  let component: CrmSmmGmailfoldersummaryComponent;
  let fixture: ComponentFixture<CrmSmmGmailfoldersummaryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailfoldersummaryComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailfoldersummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
