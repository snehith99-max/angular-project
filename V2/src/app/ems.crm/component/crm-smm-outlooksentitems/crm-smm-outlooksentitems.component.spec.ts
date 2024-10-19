import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmOutlooksentitemsComponent } from './crm-smm-outlooksentitems.component';

describe('CrmSmmOutlooksentitemsComponent', () => {
  let component: CrmSmmOutlooksentitemsComponent;
  let fixture: ComponentFixture<CrmSmmOutlooksentitemsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmOutlooksentitemsComponent]
    });
    fixture = TestBed.createComponent(CrmSmmOutlooksentitemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
