
import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnDropmarketingComponent } from './crm-trn-dropmarketing.component';

describe('CrmTrnDropmarketingComponent', () => {
  let component: CrmTrnDropmarketingComponent;
  let fixture: ComponentFixture<CrmTrnDropmarketingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnDropmarketingComponent]
    });
    fixture = TestBed.createComponent(CrmTrnDropmarketingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
