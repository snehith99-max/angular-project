import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactCorporateViewComponent } from './crm-trn-contact-corporate-view.component';

describe('CrmTrnContactCorporateViewComponent', () => {
  let component: CrmTrnContactCorporateViewComponent;
  let fixture: ComponentFixture<CrmTrnContactCorporateViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactCorporateViewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactCorporateViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
