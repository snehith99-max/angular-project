import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactCorporateaddComponent } from './crm-trn-contact-corporateadd.component';

describe('CrmTrnContactCorporateaddComponent', () => {
  let component: CrmTrnContactCorporateaddComponent;
  let fixture: ComponentFixture<CrmTrnContactCorporateaddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactCorporateaddComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactCorporateaddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
