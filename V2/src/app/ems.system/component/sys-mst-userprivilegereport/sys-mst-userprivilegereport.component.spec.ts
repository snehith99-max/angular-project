import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstUserprivilegereportComponent } from './sys-mst-userprivilegereport.component';

describe('SysMstUserprivilegereportComponent', () => {
  let component: SysMstUserprivilegereportComponent;
  let fixture: ComponentFixture<SysMstUserprivilegereportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstUserprivilegereportComponent]
    });
    fixture = TestBed.createComponent(SysMstUserprivilegereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
