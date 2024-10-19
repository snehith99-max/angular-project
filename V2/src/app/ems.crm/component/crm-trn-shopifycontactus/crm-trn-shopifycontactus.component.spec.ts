import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnShopifycontactusComponent } from './crm-trn-shopifycontactus.component';

describe('CrmTrnShopifycontactusComponent', () => {
  let component: CrmTrnShopifycontactusComponent;
  let fixture: ComponentFixture<CrmTrnShopifycontactusComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnShopifycontactusComponent]
    });
    fixture = TestBed.createComponent(CrmTrnShopifycontactusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
