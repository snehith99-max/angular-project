import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SysMstJobtypeComponent } from './sys-mst-jobtype.component';

describe('SysMstJobtypeComponent', () => {
  let component: SysMstJobtypeComponent;
  let fixture: ComponentFixture<SysMstJobtypeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SysMstJobtypeComponent]
    });
    fixture = TestBed.createComponent(SysMstJobtypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
