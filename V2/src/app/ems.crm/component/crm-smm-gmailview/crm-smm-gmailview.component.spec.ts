import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailviewComponent } from './crm-smm-gmailview.component';

describe('CrmSmmGmailviewComponent', () => {
  let component: CrmSmmGmailviewComponent;
  let fixture: ComponentFixture<CrmSmmGmailviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailviewComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
